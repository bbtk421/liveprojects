# C# Live Project

## Introduction

For the last two weeks at The Tech Academy, I worked with my fellow students in a team developing a .NET Core MVC website ultilizing Entity Framework. Our team used Azure Devops to organize the project, which I found to work well for staying organized and sending/receiving feedback on individual stories. We also used Slack for more direct communication between the team and the project manager. We started with a basic existing website and added many new requested features and troubleshooting any bugs in both the front and back ends.

Below are some descriptions, code snippets and screenshots to highlight some of my work.

## Back End Stories(most also deal with the Front End as well)
Creating the calendar event model with proper DateTime formating for this project.

```c#
public class CalendarEvent
	{
		[Key]
		public int EventId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		[DataType(DataType.Date)] // These annotations set the time and date formats
		[DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",  ApplyFormatInEditMode = true)]
		public DateTime? StartDate { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? EndDate { get; set; }
		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		public DateTime? StartTime { get; set; }
		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		public DateTime? EndTime { get; set; }
		public bool IsProduction { get; set; }
		public int? ProductionId { get; set; }
	}
```
![event screenshot 1](/csharpliveproject/event1.PNG)

```c#
@if (item.StartTime == null)
{
    <p>All Day <i class="fas fa-clock"></i></p>
}
else
{
    <p>@((item.EndDate - item.StartDate).Value.Days) Days <i class="fas fa-clock"></i></p>
```
Shows the duration of the event of hover by utilizing Bootstrap popovers.
```c#
<a tabindex="0" class="prod-calendar--pop" role="button" data-toggle="popover" data-trigger="hover" data-content="This is a @((item.EndDate - @item.StartDate).Value.TotalDays) day event.">
```
![event screenshot 2](/csharpliveproject/event2.PNG)

Here I use Razor to compare the End Date of each CalendarEvent to the current date.  If the EndDate is today, the text "Last Day!".
```c#
@if (item.EndDate == DateTime.Now.Date)
{
    <p class="prod-calendar--lastday">Last Day!</p>
}
else if (item.StartDate > DateTime.Now)
{
    <p></p>
}
else
{
    <p class="prod-calendar--daysleft">@((item.EndDate - DateTime.Now).Value.Days) days remaining!</p>
}
```

Gets the day/time spans for events or just day for all day events.
```c#
@if (item.StartTime == null)
{
    @* display span of days *@
    <text>
        @Html.DisplayFor(modelItem => item.StartDate.Value.DayOfWeek) - @Html.DisplayFor(modelItem => item.EndDate.Value.DayOfWeek)
    </text>
}
else
{
    @* display span of days and times *@
    <text>
        @Html.DisplayFor(modelItem => item.StartDate.Value.DayOfWeek) - @Html.DisplayFor(modelItem => item.EndDate.Value.DayOfWeek), @Html.DisplayFor(modelItem => item.StartTime) - @Html.DisplayFor(modelItem => item.EndTime)
    </text>
}
```
## Front End Stories
Dropleft with vertical dots icon menu for editing an entry from the index.
```c#
<div class="btn-group dropleft prod-calendar--dropleft">
	@* 3 dots menu button *@
	<a role="button" class="btn dropdown-toggle prod-calendar--leftbtn" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
		<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
			<path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0zm0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0z" />
		</svg>
	</a>
	@* 3 dots dropdown menu *@
	<ul class="dropdown-menu prod-calendar--leftmenu" aria-labelledby="dropdownMenuLink">
		<li>@Html.ActionLink("Edit", "Edit", new { id = item.EventId })</li>
		<li>@Html.ActionLink("Details", "Details", new { id = item.EventId })</li>
		<li>@Html.ActionLink("Delete", "Delete", new { id = item.EventId })</li>
	</ul>
```
![edit menu](/csharpliveproject/event3.PNG)

Responsive hamburger style nav menu with internal menu.
```html
<div class="navbar--hamburger"> <!-- this holds the mobile nav in a hamburger menu -->
	<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
		<span class="navbar-toggler-icon"></span> <!-- actual humburger button -->
	</button>
	<div class="collapse navbar-collapse" id="navbarSupportedContent">
		<ul class="navbar-nav nav">
			<li class="nav-item navbar-hamburger--navitem">
				<a class="nav-link .navbar-hamburger--navlink" href="~/Home/Index">TheaterCMS</a>
			</li>
			<li class="nav nav-item dropdown navbar-hamburger--navitem">
			<a class="nav nav-link dropdown-toggle .navbar-hamburger--navlink" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Production</a>
			<div class="dropdown-menu dropdownbckgrndclr navbar-hamburger--dropdownmenu" aria-labelledby="navbarDropdownMenuLink">
				@Html.ActionLink("Cast Members", "Index", "CastMembers", new { Area = "Production" }, new { @class = "dropdown-item dropdowntxt navbar-hamburger--dropdownitem" })
				@Html.ActionLink("Production Photos", "Index", "ProductionPhotos", new { Area = "Production" }, new { @class = "dropdown-item dropdowntxt navbar-hamburger--dropdownitem" })
				@Html.ActionLink("Productions", "Index", "Productions", new { Area = "Production" }, new { @class = "dropdown-item dropdowntxt navbar-hamburger--dropdownitem" })
			</div>
			</li>
			<li class="nav-item navbar-hamburger--navitem">
				<a class="nav-link .navbar-hamburger--navlink" href="#">Rental</a>
			</li>
			<li class="nav-item navbar-hamburger--navitem">
				<a class="nav-link .navbar-hamburger--navlink" href="#">Blog</a>
			</li>
		</ul>
	</div>
</div> <!-- mobile nav end -->
```
![mobile nav menu 1](/csharpliveproject/eventmenu1.PNG)   ![mobile nav menu 2](/csharpliveproject/eventmenu2.PNG)
