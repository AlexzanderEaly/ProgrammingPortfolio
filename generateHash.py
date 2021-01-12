# eats on webpages
from bs4 import BeautifulSoup
import hashlib
#fb407bcb4cfa6fa11460049bc7df641d
#dd3f315ea3476c89a8430ccfaf601ed4
# web driver stuff, to render dynamic webpages
import selenium
from selenium import webdriver
from selenium.webdriver.common.keys import Keys

import requests
import urllib.request
import time

import lxml

# parses the html file some more, using soup.select()
import soupsieve

#takes in the row from the DB and uses the data stored in the row to scrape a webpage, and return a md5Hash
# format of row(URLID, URL, dynamicPage, filterParameters, linkDataID, md5Hash)
def hashCodeGenMD5(row):
    if row[2]:
        # dynamic web page loader
        # do dynamicWeb.get(url) to use
        dynamicWeb = webdriver.Chrome('C:\\Users\\lcs\\Downloads\\chromedriver_win32\\chromedriver.exe')

        # gets the url for dynamic web pages
        dynamicWeb.get(row[1])

        results = dynamicWeb.page_source

        # run this after running the dyniamic one
        soup = BeautifulSoup(results, 'lxml')
    else:
        # actually gets the url
        # for no dynamic websites
        response = requests.get(row[1])

        # run this after running the static one
        soup = BeautifulSoup(response.text, 'lxml')
        # end of if!!!!!

        # parses/filters out the stuff that we dont want table:is(#requestTable) tbody tr td :not(.has-tip)
    file = soup.select(row[3])


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

    # puts the encoding of the parsed HTML file into utf-8 so i can hash it
    htmlStuff = htmlStuff.encode(encoding='utf-8', errors='strict')

    # hashs the HTML file
    return hashlib.md5(htmlStuff).hexdigest();