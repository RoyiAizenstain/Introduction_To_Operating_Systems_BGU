@echo off
setlocal enabledelayedexpansion

rem Request user input for the number of processes and iterations
echo Enter the number of processes (how many processes to run in parallel):
set /p count=

echo Enter the number of iterations (iterations per process):
set /p iterations=

rem Define variable for sum of times
set sum=0

rem Define the path to CPU-Process.exe
set exePath=C:\Users\u2\source\repos\CPU-Process\CPU-Process\bin\Debug\net8.0\CPU-Process.exe

rem Run the specified number of processes concurrently and measure the time for each
for /L %%i in (1,1,%count%) do (
    rem Record start time
    for /F "tokens=1-4 delims=:.," %%a in ('echo %time%') do (
        set startHour=%%a
        set startMinute=%%b
        set startSecond=%%c
        set startMilliSecond=%%d
    )
    
    rem Start the process
    start /B "" "%exePath%" %iterations%

    rem Record end time
    for /F "tokens=1-4 delims=:.," %%a in ('echo %time%') do (
        set endHour=%%a
        set endMinute=%%b
        set endSecond=%%c
        set endMilliSecond=%%d
    )

    rem Calculate the difference in time (convert to milliseconds)
    set /A startTotalMs = (10000 * %startHour% + 100 * %startMinute% + %startSecond%) * 100 + %startMilliSecond%
    set /A endTotalMs = (10000 * %endHour% + 100 * %endMinute% + %endSecond%) * 100 + %endMilliSecond%
    
    set /A elapsedTimeMs = %endTotalMs% - %startTotalMs%
    
    rem Add elapsed time to sum
    set /A sum=!sum! + !elapsedTimeMs!
)

rem Calculate average time
set /A averageTime=!sum! / %count%

rem Display the average time
echo Average time for %count% processes: %averageTime% ms

endlocal
pause