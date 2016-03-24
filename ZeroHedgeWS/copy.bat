SET mypath=%~dp0
echo %mypath:~0,-1%

@echo %mypath%

mkdir "%mypath%Scripts\WebSharper\Application"

copy /Y "%mypath%Scripts\jquery-2.2.0.min.js" "%mypath%Scripts\WebSharper\Application"
copy /Y "%mypath%Scripts\bootstrap.min.js" "%mypath%Scripts\WebSharper\Application"
copy /Y "%mypath%Scripts\jquery.smartmenus.js" "%mypath%Scripts\WebSharper\Application"
copy /Y "%mypath%Scripts\jquery.smartmenus.bootstrap.js" "%mypath%Scripts\WebSharper\Application"


mkdir "%mypath%Content\WebSharper\Application
copy /Y "%mypath%Content\favicon.ico" "%mypath%Content\WebSharper\Application"
copy /Y "%mypath%Content\css\bootstrap.min.css" "%mypath%Content\WebSharper\Application"
copy /Y "%mypath%Content\css\bootstrap-theme.min.css" "%mypath%Content\WebSharper\Application"
copy /Y "%mypath%Content\css\styles.css" "%mypath%Content\WebSharper\Application"
copy /Y "%mypath%Content\css\jquery.smartmenus.bootstrap.css" "%mypath%Content\WebSharper\Application"