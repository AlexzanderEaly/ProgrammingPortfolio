import pyodbc
import generateHash
import crwalLinksDeep

"""
MY DB MSSQL  connection String

"Driver={SQL Server};"
"Server=LCS-PC;"
"Database=WebSystem;"
"Trusted_Connection=yes;"



"""
#connection string used to conected to DB
conString = ("Driver={SQL Server};"
                      "Server=LCS-PC;"
                      "Database=WebSystem;"
                      "Trusted_Connection=yes;")


def crwalLinks(linkID, listOfLinks):

    conString1 = ("Driver=MariaDB ODBC 3.1 Driver;"
                  "Server=127.0.0.1;"
                  "USER=root;"
                  "Database=buildersexchange;"
                  "PORT=3306")

    crwalLinksTrue = False

    linksFound = 0

    dbLink = pyodbc.connect(conString)

    dbPointer = dbLink.cursor()

    # holds the urls that will be scraped that wasnt added to the database by the admins.
    cleanLinks = []

    for linksFound in listOfLinks:
        cleanLinks.append(linksFound.get('href'))

        # some fuction that takes the link in and try to add it to the database
    for links in cleanLinks:
        try:

            dbPointer.execute("INSERT INTO linksFound(linkURL, linkID) VALUES(?,?)", (links, linkID))
            dbPointer.commit()
        except:
            print("LINK IS IN DB")

    dbPointer.execute(
        'SELECT linksFound.linkID, linksFound.linkURL, linkOptions.pageIsDynamic, linkOptions.filterParamerters, linkOptions.crwalLinks ,linksFound.md5Hash ' +
        ' FROM linksFound JOIN crwalFilter ON linksFound.linkID = crwalFilter.linkID JOIN linkOptions  ON linkOptions.filterID = crwalFilter.filterID ' +
        ' WHERE linksFound.linkID = ?', (linkID))

    listOfUrlRecords = []
    for row in dbPointer:
        rows = []
        for rowItem in row:
            rows.append(rowItem)
        listOfUrlRecords.append(rows)

    dbPointer.close()
    dbLink.close()

    deepCrwal = listOfUrlRecords[1][4]
    if deepCrwal:
        crwalLinksTrue = 1

        for linkList in listOfUrlRecords:
            crwalLinksDeep.crwalLinksDeep(linkList[0], linkList[1], linkList[2], linkList[3])

        dbLink = pyodbc.connect(conString)

        dbPointer = dbLink.cursor()

        dbPointer.execute(
            'SELECT linksFound.linkID, linksFound.linkURL, linkOptions.pageIsDynamic, linkOptions.filterParamerters, linkOptions.crwalLinks ,linksFound.md5Hash ' +
            ' FROM linksFound JOIN crwalFilter ON linksFound.linkID = crwalFilter.linkID JOIN linkOptions  ON linkOptions.filterID = crwalFilter.filterID ' +
            ' WHERE linksFound.linkID = ?', (linkID))
        listOfUrlRecords = []
        for row in dbPointer:
            rows = []
            for rowItem in row:
                rows.append(rowItem)
            listOfUrlRecords.append(rows)

        dbPointer.close()
        dbLink.close()

        for linkList in listOfUrlRecords:
            generateHash.hashCodeGenMD5(linkList, crwalLinksTrue, 1)

    else:
        dbPointer.close()
        dbLink.close()
        crwalLinksTrue = 0
        for linkList in listOfUrlRecords:
            generateHash.hashCodeGenMD5(linkList, crwalLinksTrue, 1)
