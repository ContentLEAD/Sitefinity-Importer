File Locations:
NOTE: all filenames in "quotes" indicate a directory...all were found only named "Brafton"; I added the rest of the name for clarity

"Brafton - Control Templates" (entire directory) : {site_root}/Sitefinity/ControlTemplates
braftonInsert.aspx & braftonInsert.aspx.cs : {site_root}

BraftonCategory.cs : {site_root}/App_Code
BraftonDateArchive.cs : {site_root}/App_Code
BraftonFeed.cs : {site_root}/App_Code
BraftonFeedDetail.cs : {site_root}/App_Code

BraftonAPI.dll : {site_root}/bin
BraftonData.dll : {site_root}/bin

"Brafton - bin" (entire directory) : {site_root}/bin

"Brafton - controls" (entire directory) : {site_root}/Controls


web.config (in {site_root}...I've included a copy of it as a reference and in case I missed anything )

**the following rewrite rules must be added...they allow variables to be passed to the page (such as whether it's a single article or archive view)**
<urlrewrites>
	<!-- Add your rule elements here -->
	<rule>
		<url>^([A-Za-z-/]*)?news-item/-in/([A-Za-z0-9-]+)/([A-Za-z0-9-]+)/?(\.aspx)?</url>
		<rewrite>$1news-item.aspx?cat=$2&amp;article=$3&amp;mode=Detail</rewrite>
	</rule>
	<rule>
		<url>^([A-Za-z-/]*)?news-item/-in/([A-Za-z0-9-]+)/?(\.aspx)?(\?(.+))$</url>
		<rewrite>$1news-item.aspx?cat=$2&amp;$5</rewrite>
	</rule>
	<rule>
		<url>^([A-Za-z-/]*)?news-item/-in/([A-Za-z0-9-]+)/?(\.aspx)?</url>
		<rewrite>$1news-item.aspx?cat=$2</rewrite>
	</rule>
	<!-- Rewrite Rule for single article links on archive page -->
	<rule>
		<url>^([A-Za-z-/]*)?news-item/-on/([0-9-]+)/([0-9-]+)/([A-Za-z0-9-]+)/?(\.aspx)?</url>
		<rewrite>$1news-item.aspx?article=$4&amp;mode=Detail</rewrite>
	</rule>
	<!-- Date Archive URL Structure: -->
	<rule>
		<url>^([A-Za-z-/]*)?news-item/-on/([0-9-]+)/([0-9-]+)/?(\.aspx)?</url>
		<rewrite>$1news-item.aspx?year=$2&amp;month=$3</rewrite>
	</rule>
	<rule>
		<url>^([A-Za-z-/]*)?news-item/([A-Za-z0-9-]+)/?(\.aspx)?</url>
		<rewrite>$1news-item.aspx?article=$2&amp;mode=Detail</rewrite>
	</rule>
</urlrewrites>

**the following must be added to the "toolboxControls" tag, registers controls to be added to a page via Sitefinity's backend**:
<add name="Brafton News" section="Content" type="Webality.BraftonFeed, App_Code" description="Used to display content pulled in from Brafton."/>
<add name="Brafton News Detail" section="Content" type="Webality.BraftonFeedDetail, App_Code" description="Used to display content pulled in from Brafton."/>
<add name="Brafton Categories" section="Content" type="Webality.BraftonCategory, App_Code" description="Used to display categories from Brafton."/>
<add name="Brafton Date Archives" section="Content" type="Webality.BraftonDateArchive, App_Code" description="Used to display date archives from Brafton."/>



File purposes:
**I'll only be stating the filename...other than dlls, it's understood that there's a .aspx and a .aspx.cs file for each**

BraftonCategory : generates a list of categories to be displayed in the sidebar
BraftonDateArchive : generates a date archive list in the format "Month Year", to be displayed in sidebar
BraftonFeedDetail : SHOULD generate a single article view
BraftonFeed : Main article list page...generates either a single list of articles (for date and category archives), or a standard two column view, broken down by category, for the news landing page
BraftonFeed_HomeTemplate : I believe this is an older version of BraftonFeed, left behind by GrayWeb tech people, included for the sake of being thorough

BraftonInsert : displays HoH

BraftonAPI.dll / BraftonData.dll : Honestly not sure what the difference is between these two, though I know they're responsible for creating a "BraftonNews" structure that holds all relevant article info

"Braton - bin" files : I frankly have literally no idea what purpose these serve in the greater whole...however, they were stored in a folder named "Brafton", so I assume they're significant