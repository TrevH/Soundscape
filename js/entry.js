$(document).ready(function()
{    
	console.log("Hello from entry");
	$(".overlay").click(function()
	{
		var pagename = $(this).data("pagename");
		console.log(pagename);
		if (pagename == "report_shiva" || pagename == "report_trevor")
		{
			window.location.href = pagename + ".html";
		}
		else
		{
			localStorage.setItem("selectedPage", pagename);
			console.log(localStorage.selectedPage);
			window.location.href = "page2.html";	
		}

	})
});
