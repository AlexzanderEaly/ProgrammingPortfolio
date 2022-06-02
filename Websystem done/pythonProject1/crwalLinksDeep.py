import generateHash
import pyodbc
from bs4 import BeautifulSoup

#connection string used to conected to DB
conString = ("Driver={SQL Server};"
                      "Server=LCS-PC;"
                      "Database=WebSystem;"
                      "Trusted_Connection=yes;")



def crwalLinksDeep(linkID, url,dynamic,urlFilter):


    soup = generateHash.MakeSoup(dynamic, url)

    listOfLinks = soup.select(urlFilter + ' a:not([href$=pdf]):is([href^=http])')

    dbLink = pyodbc.connect(conString)

    cleanLinks = []

    linkInDB = 0

    for linksFound in listOfLinks:
        cleanLinks.append([linksFound.get('href'),0])

        #some fuction that takes the link in and try to add it to the database
    for links in cleanLinks:
        try:
            with dbLink:
                dbPointer = dbLink.cursor()

                dbPointer.execute("INSERT INTO linksFound(linkURL, linkID) VALUES(?,?)", (links[0], linkID))
                dbPointer.commit()
                print("new Link " + links[0])
        except:
            print("LINK IS IN DB" + links[0])
            #used to keep track of how many old links are found on the page
            linkInDB += 1
            #keeps track of any link that is old and thus should not be crwaled again for new links.
            links[1] = 1

        #tells scrpit to go to the next link if this page has no links to crwal or there are no new links added to the current page
        if linkInDB == len(cleanLinks) or len(cleanLinks) == 0:
            continue;

        #tell when a new link has been found and that we should search it for more links
        if links[1] != 1:
            crwalLinksDeep(linkID,links[0],dynamic,urlFilter)

