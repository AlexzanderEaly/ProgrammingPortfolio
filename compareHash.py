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


# takes the row, and hashcode that was genarated by the hashCodeGenMD5 module, and decides if the link has changed
# if so then it will open a new connection to the database using the pydoc object passed to it to gets a list of
# user's email and then uses that list to send an email to each user, notfiying them that the link has changed
def compareHash(row, hashcode):
    # tells us to send the email or not
    if hashcode == row[5]:
        print(hashcode)

    # sends the email to the designated user when the link has changed
    else:

        dbLink = pyodbc.connect("Driver={SQL Server};"
                                 "Server=LCS-PC;"
                                 "Database=WebSystem;"
                                 "Trusted_Connection=yes;")


        # turns the urlID into a string for the SQL query
        urlIDString = str(row[0])

        # allows for query execution
        userEmailList = dbLink.cursor()

        # gets list of user's email that have the link assigned to them
        userEmailList.execute(
            'SELECT email FROM dbo.users WHERE dbo.users.userID IN (SELECT userID FROM assignUser WHERE linkID =' + urlIDString + ')')

        # sends each user an email instead of one big group email.
        for email in userEmailList:
            # basucally loging in to start sending the emails bb
            sendIt = yagmail.SMTP('alexjobstest@gmail.com')

            # the email of the user
            emailStr = str(email)

            # formats the email to accptable standers
            emailStr = emailStr.replace("'", '').replace(",", '').replace(" ", '').replace(")", '').replace("(", '')

            # tester print
            print(emailStr)

            # SENDS THE EMAIL
            sendIt.send(to=emailStr, subject='Link Updated!!!', contents='The link has Changed \n' + row[1])
        # End of For
        userEmailList.close()
        # used to update the MD5 hash for the next time
        hashQuery = dbLink.cursor()
        # update it
        hashQuery.execute(
            "UPDATE WebSystem.dbo.linkData SET md5Hash = '" + hashcode + "' WHERE WebSystem.dbo.linkData.linkID = " + urlIDString)

        # commits changes to the database
        dbLink.commit()

        # opens the connection for next query by closing this one out
        hashQuery.close()

    # end of IF

# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy
# Made by Alexzander Ealy
