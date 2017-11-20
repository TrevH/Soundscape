$(document).ready(function()
{    
	console.log("Hello from entry.js");
	$(".overlay").click(function()
	{
		var pagename = $(this).data("pagename");
		console.log(pagename);
		localStorage.setItem("selectedPage", pagename);
		console.log(localStorage.selectedPage);
		window.location.href = "page2.html";
	})
});
