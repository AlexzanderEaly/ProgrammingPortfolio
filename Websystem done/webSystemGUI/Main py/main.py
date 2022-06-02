import sys
from selenium import webdriver
import requests
from bs4 import BeautifulSoup
import lxml

def MakeSoup(isDynamic, url):
    if isDynamic:
        # dynamic web page loader
        # do dynamicWeb.get(url) to use
        dynamicWeb = webdriver.Chrome('C:\\Program Files (x86)\\Google\\Chrome\\Application\\chromedriver.exe')

        # gets the url for dynamic web pages
        dynamicWeb.get(url)

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

    return soup


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
    return htmlStuff

#this runs the moduels defined above and returns the output to the GUI
if __name__ == '__main__':
    #sets up the string url from the link textbox
    url = sys.argv[1]
    #sets up the string filter from the filter textbox
    filter = sys.argv[2]

    #takes the string and based on what it is evaluatus it to the bool version.
    if(sys.argv[3] == "1"):
        dynamic = True
    else:
        dynamic = False

#makes a soup object with the url
previewSoup = MakeSoup(dynamic, url)

#filters the soup object to just be the html that the user wishes to monitor.
previewFilter = MakeFilteredSoup(filter, previewSoup)

previewFilter = previewFilter.encode(encoding='ascii', errors = 'replace')

print(previewFilter.decode(encoding='utf-8', errors = 'strict'))
# See PyCharm help at https://www.jetbrains.com/help/pycharm/