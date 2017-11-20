$(document).ready(function()
{    
	var pageselected = localStorage.getItem('selectedPage');
	console.log(pageselected);

	if(pageselected===null)
	{
		$('#content-section-block').load("overview.html");
	}
	else if(pageselected=='report_shiva')
	{
		 window.location.href = 'report_shiva.html';
	}
	else if(pageselected=='report_trevor')
	{
		window.location.href = 'report_trevor.html';
	}
	else
	{
		$('#content-section-block').load(pageselected + '.html');
	}
	

	$('.page2-menu-options').on('click', function(e)
	{
        e.preventDefault();
		var page = $(this).attr('href');

		if(page == 'shiva_report')
		{
			window.location.href = 'report_shiva.html';
		}
		else if (page == 'trevor_report')
		{
			window.location.href = 'report_trevor.html';
		}
		else
		{
			$('#content-section-block').load(page + '.html');
		}
		
        return false;
	});
});
