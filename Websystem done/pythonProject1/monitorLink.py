


#takes in the row from the DB and uses the data stored in the row to scrape a webpage, and return a md5Hash
# format of row(URLID, URL, dynamicPage, filterParameters, linkDataID, md5Hash)
import generateHash

# takes the row, and hashcode that was genarated by the hashCodeGenMD5 module, and decides if the link has changed
# if so then it will open a new connection to the database using the pydoc object passed to it to gets a list of
# user's email and then uses that list to send an email to each user, notfiying them that the link has changed
import compareHash

import crwaledLinksList
# access the database
import pyodbc

def checkLink(urlRecord):


    # takes in the row from the DB and uses the data stored in the row to scrape a webpage, and return a md5Hash
        # format of row(URLID, URL, dynamicPage, filterParameters, linkDataID, md5Hash)
    generateHash.hashCodeGenMD5(urlRecord)
