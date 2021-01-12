/*Alexzander Ealy 11/7-12/4/19
Dr. John Kinuthia
ISYS 288-001
does all the form validation for my website*/

//fills my selection list
var selectionList = ["Pick a Goverment", "Dictator", "Democracy", "Communism", "Other"];
var list = document.getElementById("fillThis");
fillList();

function fillList()
{
    var select = document.getElementById("fillThis");

    for(var i = 0; i < 5;i++)
        {
        select.options[select.options.length] = new Option(selectionList[i].toString(), "Gov" + i.toString());
        }

}

//makes sure all the boxes are filled and if so then displays some data
function valid()
{
    var userName = document.getElementById("userName").value;
    
    //brings the list into the javascript
    var list = document.getElementById("fillThis");
    //takes the item picked and responds
    var listItemTxt = list.options[list.selectedIndex].text;
    
    //gets the value of the item picked from the list
    var listItemValue = list.options[list.selectedIndex].value;
    
    var status = document.getElementsByName("status");
    
    var statusValue = "no value";
    
    var date = document.getElementById("powerDate").value;
    
    var formFilled = true;
    
    for(var counter = 0;counter < status.length;counter++)
        {
            if(status[counter].checked)
                {
                    statusValue = status[counter].value;
                }
        }

    //the if's check to see if the form has been filled
    if(userName == "")
        {
            alert("Please Enter your Name!");
            document.getElementById("userNameLabel").style.color = "red";
            formFilled = false;
        }
    
    if(date == "")
        {
            alert("Please Pick A Date!");
            document.getElementById("powerDateLabel").style.color = "red";
            formFilled = false;
        }
    
    if(listItemValue == "Gov0")
        {
            alert("Please Pick A Goverment!");
            document.getElementById("listLabel").style.color = "red";
            formFilled = false;
        }
    
    if(formFilled)
        {
            alert("userName: " + userName +
                  "\nType Of Goverment: " + listItemTxt +
                  "\nDate to Come To Power: " + date +
                  "\nWhat you will be when you come to Power: " + statusValue +
                  "\nPower Score " + getScore());
            
        }
    
    //simple function to return a string with thier power score
    function getScore()
    {
        if(listItemValue == "Gov1" && statusValue == "Dictator")
            {
                return "Over 9000";
            }
        else if(listItemValue == "Gov3" && statusValue == "Presdient")
            {
                return "20";
            }
        else if(listItemValue == "Gov2" && statusValue == "Presdient")
            {
                return "-9000";
            }
        else
        {
            return "0";
        }
    }
    
}

