# eats on webpages
from bs4 import BeautifulSoup

import pyodbc

#does the hashing
import hashlib

# web driver stuff, to render dynamic webpages
import selenium
from selenium import webdriver
from selenium.webdriver.common.keys import Keys

#gets the web pages for static pages
import requests
import urllib.request

#used by beatufilSoup to make parsing fast
import lxml

# parses the html file some more, using soup.select()
import soupsieve

import crwalPDF

#connection string used to conected to DB
conString = ("Driver={SQL Server};"
                      "Server=LCS-PC;"
                      "Database=WebSystem;"
                      "Trusted_Connection=yes;")

# takes the row, and hashcode that was genarated by the hashCodeGenMD5 module, and decides if the link has changed
# if so then it will open a new connection to the database using the pydoc object passed to it to gets a list of
# user's email and then uses that list to send an email to each user, notfiying them that the link has changed
import compareHash

#gets all the links to other webpages off the webpage for a predefined area.
import crwaledLinksList

# takes in the row from the DB and uses the data stored in the row to scrape a webpage, and return a md5Hash
# format of row(URLID, URL, dynamicPage, filterParameters, linkDataID, md5Hash)
def hashCodeGenMD5(row, crwalLinksTrue = 1, linksCrwaled = 0):

    soup = MakeSoup(row[2], row[1])


    htmlStuff = MakeFilteredSoup(row[3], soup)


    if(htmlStuff == ""):
        #call spcial email.
        print("HELKLOIO FITTER DIDNT WORK")
        compareHash.compareHash(row, "1223", linksCrwaled, "", True)
        pass;
    else:
        hashcode = GetHash(htmlStuff)

        #htmlStuff = htmlStuff.decode(encoding='utf-8', errors='strict')

        emailHTML = soup.find_all('html')
        areaFiltered = soup.select(row[3])

        for tag in areaFiltered:
            tag['style'] = "background-color:red;"
            highlightedArea = str(tag)

        print(highlightedArea)

        htmlTagString = ""
        #for html in emailHTML:
        print(type(emailHTML))
        print(emailHTML)
        htmlTagString += str(emailHTML)

        emailHTML = htmlTagString.replace(htmlStuff,highlightedArea)



        # takes the row, and hashcode that was genarated by the hashCodeGenMD5 module, and decides if the link has changed
        # if so then it will open a new connection to the database using the pydoc object passed to it to gets a list of
        # user's email and then uses that list to send an email to each user, notfiying them that the link has changed
        compareHash.compareHash(row, hashcode, linksCrwaled, emailHTML)

        SetHash(hashcode, linksCrwaled, row)

        if row[4] and crwalLinksTrue == 1 and linksCrwaled != 1:
            soupFiltered = soup.select(row[3] + ' a:not([href$=pdf]):is([href^=http])')
            crwaledLinksList.crwalLinks(row[0], soupFiltered)
        else:
            print("we not crwaling")

#####################################
######################################
####################################
#####################################
#########################################################################
######################################
####################################
#####################################
####################################

def SetHash(hashcode, linksCrwaled, row):
    dbLink = pyodbc.connect(conString)
    hashQuery = dbLink.cursor()
    if linksCrwaled:
        hashQuery.execute("UPDATE linksFound SET linksFound.md5Hash ='"+ hashcode + "'WHERE linksFound.linkURL ='"+ row[1] + "'")
        # commits changes to the database
        hashQuery.commit()
        print("commited")
        # opens the connection for next query by closing this one out
        hashQuery.close()
        dbLink.close()
    else:
        # turns the urlID into a string for the SQL query
        urlIDString = str(row[0])

        hashQuery.execute(
            "UPDATE WebSystem.dbo.linkData SET md5Hash = '" + hashcode + "' WHERE WebSystem.dbo.linkData.linkID = " + urlIDString)
        # commits changes to the database
        hashQuery.commit()

        # opens the connection for next query by closing this one out
        hashQuery.close()
        dbLink.close()

def GetHash(htmlStuff):
    # puts the encoding of the parsed HTML file into utf-8 so i can hash it
    htmlStuff = htmlStuff.encode(encoding='utf-8', errors='strict')
    # hashs the HTML file
    hashcode = hashlib.md5(htmlStuff).hexdigest();
    return hashcode


def MakeFilteredSoup(filter, soup):
    # parses/filters out the stuff that we dont want table:is(#requestTable) tbody tr td :not(.has-tip)
    file = soup.select(filter)
    # will hold the pasred version of the html page before its passed to another var for it to be turned to utf-8
    newFile = []
    # keeps track of how many iterations have happend
    counter = 0
    # Sets the max number of iterations
    stopAT = len(file)
    # makes the pop happend at the start so it goes from top to bottom
    index = 0
    while counter < stopAT:
        newFileStr = file.pop(index)
        # incrents the count so we dont go over tha max number of iterations
        counter += 1;
        # adds it to the newFile list array of strings
        newFile.append(str(newFileStr))
        # sets the newFileStr to "" which clears it for the next line
        newFileStr = ""
    # endWhile
    # will hold what was scraped fromm the HTML file after being parsed
    htmlStuff = ""
    for string in newFile:
        htmlStuff += string

    print(htmlStuff)

    return htmlStuff


def MakeSoup(isDynamic, url):
    try:
        if isDynamic:
            # dynamic web page loader
            # do dynamicWeb.get(url) to use
            dynamicWeb = webdriver.Chrome('C:\\Program Files (x86)\\Google\\Chrome\\Application\\chromedriver.exe')

            # gets the url for dynamic web pages
            dynamicWeb.get(url)

            #a way to check for a status 200
            response = requests.get(url)

            results = dynamicWeb.page_source

            dynamicWeb.close()
            # run this after running the dyniamic one
            soup = BeautifulSoup(results, 'lxml')
        else:
            # actually gets the url
            # for no dynamic websites
            response = requests.get(url)

            # run this after running the static one
            soup = BeautifulSoup(response.text, 'lxml')
            # end of if!!!!!
    except:
        soup = BeautifulSoup("",'lxml')
        return soup

    if(response.status_code != 200):
        soup = BeautifulSoup("", 'lxml')
        return soup

    return soup

#  if True:
  #      crwalPDF.crwalPDF(soup.select(row[3] +' a:is([href^=".pdf"])'))