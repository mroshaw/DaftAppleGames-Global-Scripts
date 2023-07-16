@echo off
@echo:
@echo ***** DAFT APPLE GAMES *****
@echo ****************************
@echo:
@echo *** PREPARE NEW PROJECT ***
@echo:
:getpipeline
rem Determine pipeline
set "PIPELINE="
set /p "PIPELINE=Project render pipeline (B)RIP, (H)DRP, (U)RP: "
if "%PIPELINE%" == "B" goto gettarget
if "%PIPELINE%" == "H" goto gettarget
if "%PIPELINE%" == "U" goto gettarget
@echo Invalid pipeline entry.
goto getpipeline

:gettarget
set "ASSETTARGET="
set /p "ASSETTARGET=Project Assets path (empty to quit): "
if [%ASSETTARGET%]==[] goto abort
if exist "%ASSETTARGET%\" goto setupscripts
@echo Project asset folder does not exist
goto gettarget

:setupscripts
@echo Setting up Scripts
call CreateJunctionLinks_Scripts.bat %ASSETTARGET%
@echo Setting up Asset Resources
call CreateJunctionLinks_Assets.bat %ASSETTARGET% %PIPELINE%
goto done

:abort
@echo Aborted
goto end

:done
@echo Done
@pause

:end
