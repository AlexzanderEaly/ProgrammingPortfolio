/*Alexzander Ealy 12/1-12/4/19
Dr. John Kinuthia
ISYS 288-001
slideshow for stalin*/
/*img slideshow*/
var count = 1;
var ok = setInterval(slideShow, 4000);
function slideShow()
{
    
        document.getElementById("leaderImg").setAttribute("src", "stalin/stalin" + count.toString()+ ".jpg");
        ++count;
    
    if(count == 4)
        {
            count = 1;
        }
}
