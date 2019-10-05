git status

@echo off

FOR /F "tokens=1,2,3 delims=/ " %%a in ("%DATE%") do (
set DIA=%%a
set MES=%%b
set ANO=%%c
)

FOR /F "tokens=1,2,3 delims=:, " %%a in ("%TIME%") do (
set H=%%a
set M=%%b
set S=%%c
)

[B]set H=0%H%
set H=%H:~-2%[/B]

set FORMATO=%ANO%_%MES%_%DIA%__%H%_%M%_%S%

echo %FORMATO%

git add .

git commit -m %FORMATO%

git push


pause