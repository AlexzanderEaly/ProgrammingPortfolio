# scans a url that is pulled from a database,
# then filters the webpages html,
# then md5 hashs the webpage, and
# then if the hash differs from last time it will
# send an email to the users assigned to that link
# using the database to get the emails searching them by
# linkID which is then linked to UserID which can be used
# to get the emails finally updates the database with
# the new md5 hash that was genrated this time around

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
# Made by Alexzander Ealy
# doing DFD 1.2.1-3

import requests
from bs4 import BeautifulSoup
import urllib.request
import time
import timeit

start = time.time()

# runs multiple versions of the script below so i may get work done faster
import concurrent.futures

# just makes the hash, based on the web pages after its been filtered.
import generateHash

# module used to compare hashvalues genarted by the generateHash module
import compareHash
# access the database
import pyodbc

# is the module that combines genrating a hash, and comparing a hash into one.
import monitorLink
if __name__ == '__main__':


    #connection string used
    conString = ("Driver={SQL Server};"
                      "Server=LCS-PC;"
                      "Database=WebSystem;"
                      "Trusted_Connection=yes;")

    # creates the Link between my python and the SQL server database
    sqlLink = pyodbc.connect(conString)
    # creates a executor that can run SQL query
    urlRecord = sqlLink.cursor()

    # runs the query
    urlRecord.execute('SELECT URLS.linkID, link, linkOptions.pageIsDynamic , filterParamerters, crwalLinks, md5Hash ' +
                      ' FROM URLS JOIN linkOptions ON URLS.filterID = linkOptions.filterID JOIN linkData ON URLS.linkID = linkData.linkID ' +
                      ' WHERE linkOptions.monitorRate = 2')
    # stores all the results in a list so I can free up the executor to run other queries
    listOfUrlRecords = []
    for row in urlRecord:
        rows = []
        for rowItem in row:
            rows.append(rowItem)
        listOfUrlRecords.append(rows)
    # closing the connection for a new one to be open
    urlRecord.close()
    sqlLink.close()

    print(listOfUrlRecords)

    # runs through the list made above and allows the code to genrate a new hash for each link
    # if the link has changed it will send a email and update the md5 Hash in the database

    with concurrent.futures.ThreadPoolExecutor() as executor:
        # testing input:
        executor.map(monitorLink.checkLink, listOfUrlRecords)
        print('done')
    # end Of For

    end = time.time()

    print(end - start)

    # Alexzander ealy
