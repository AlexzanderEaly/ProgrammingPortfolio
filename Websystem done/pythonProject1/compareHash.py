# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy

# sends the email, when the webpage changes
import yagmail

# access the database
import pyodbc

# used to securely store password
import keyring

from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText


#connection string used to conected to DB
conString = ("Driver={SQL Server};"
                      "Server=LCS-PC;"
                      "Database=WebSystem;"
                      "Trusted_Connection=yes;")


# takes the row, and hashcode that was genarated by the hashCodeGenMD5 module, and decides if the link has changed
# if so then it will open a new connection to the database using the pydoc object passed to it to gets a list of
# user's email and then uses that list to send an email to each user, notfiying them that the link has changed
def compareHash(row , hashcode, linksCrwaled, file, isError = False):
    # tells us to send the email or not


    if hashcode == row[5] and isError is not True:
        print(hashcode)

    # sends the email to the designated user when the link has changed
    else:

        dbLink = pyodbc.connect(conString)


        # turns the urlID into a string for the SQL query
        urlIDString = str(row[0])

        #file = file.decode('utf-8')

        # allows for query execution
        userEmailList = dbLink.cursor()

        # gets list of user's email that have the link assigned to them
        userEmailList.execute(
            'SELECT email FROM dbo.users WHERE dbo.users.userID IN (SELECT userID FROM assignUser WHERE assignUser.secID IN (SELECT assignLink.secID FROM assignLink where assignLink.linkID =' + urlIDString + '))')

        emailList = []

        #creates a list of emails currently assigned to the link
        for email in userEmailList:
            emailList.append(email)

        # SENDS THE EMAIL
        # depending on if the isError value it will send
        # error message to user if isError is true
        # otherwise sends a link updated
        if (isError):
            file += 'The system failed to scan the page. Please review website manually website link is:' + row[1] + '\n '
        else:
            file += 'The page below has change look in the area highlighted in red to find changes. website link is:' + row[1] + '\n please note: if page doesn\'t look as expected or red area is not in the right place please notify system Admin.'

        # sends each user an email instead of one big group email.
        for email in emailList:
            # basucally loging in to start sending the emails bb
            sendIt = yagmail.SMTP('alexjobstest@gmail.com')

            # the email of the user
            emailStr = str(email)

            # formats the email to accptable standers
            emailStr = emailStr.replace("'", '').replace(",", '').replace(" ", '').replace(")", '').replace("(", '')


            # tester print
            print(emailStr)



            #sends email with a unqiue header based on if the link had an error or not
            if (isError):
                sendIt.send(to=emailStr, subject='LINK FAILURE!!!' + row[1], contents=file)
            else:
                sendIt.send(to=emailStr, subject='Link Updated!!! ' + row[1], contents=file)

        userEmailList.close()
        dbLink.close()


    # end of IF



































































# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy
