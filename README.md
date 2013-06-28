CssDownloader
=============

CSSDownloader is a simple app written in c#. It does one simple thing: downloads the given css file with images referenced in it.

Usage is simple too. just open command prompt and type

  cssd http://www.example.com/yourCssFile.css
  
and the app will download the css file with images, in a folder named same as the file.

There are lots of things to do such as:

* App will download the css file and contents in the same folder of application itself. Maybe it should download to current directory?
* Html support. Users should provide a url of an html and the app should find all the css files, and do the same job (downloading images) for each.


Why i wrote an application for that?
====================================
Because i am using windows, and don't want to install either cygwin etc. to use wget. And also i don't want to install firefox browser.
