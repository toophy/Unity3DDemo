@echo off  
rem �����ļ�  
for /f "delims=" %%i in ('dir /b "Proto\*.proto"') do echo %%i  
rem תcpp  for /f "delims=" %%i in ('dir /b/a "*.proto"') do protoc -I=. --cpp_out=. %%i  
for /f "delims=" %%i in ('dir /b/a "Proto\*.proto"') do protogen -i:Proto\%%i -o:Message\%%~ni.cs  
pause