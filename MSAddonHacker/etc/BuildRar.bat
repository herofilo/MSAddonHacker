
set OUTPATH=%1
set ARCHIVE=%2
set TARGET=%3
cd %OUTPATH%
REM "c:\archivos de programa\winrar\rar.exe" a  %ARCHIVE%  %TARGET% *.dll
del /Q %ARCHIVE%
"c:\archivos de programa\winrar\winrar.exe" a -afzip %ARCHIVE%  %TARGET% *.dll README.*

if exist %ARCHIVE% goto movedistribution
goto end
:movedistribution

move %ARCHIVE% ..\..\..\Distribution

:end

exit 0