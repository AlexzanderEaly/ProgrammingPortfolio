/*Alexzander Ealy 12/1-12/4/19
Dr. John Kinuthia
ISYS 288-001
slideshow for Kim*/
/*img slideshow*/
var count = 1;
slideShow();
var ok = setInterval(slideShow, 4000);
function slideShow()
{
    
        document.getElementById("leaderImg").setAttribute("src", "kim/kim" + count.toString()+ ".jpg");
        ++count;
    
    if(count == 3)
        {
            count = 1;
        }
}
