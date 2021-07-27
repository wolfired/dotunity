#!/bin/env bash

source $(dirname $0)/addrsrc_inc.sh



#---------------------------------------------------------------------------------------------------
#                                                                               create unity project
# UnityCreateProject $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj



#---------------------------------------------------------------------------------------------------
#                                                                                new dotnet solution
DotSolutionNew $path_sln $name_sln



#---------------------------------------------------------------------------------------------------
#                          dotnet console project, convert unity csproj files to dotnet csproj files
name_dotprj=u3dcsprojcloner
DotProjectNew $path_sln console "net5.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotBuild $path_sln/$name_dotprj $path_out_dot



#---------------------------------------------------------------------------------------------------
#                                     dotnet classlib project, unity plugin, no unity package depend
name_dotprj=nopack_depend_player
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor



#---------------------------------------------------------------------------------------------------
#                              dotnet classlib project, unity editor plugin, no unity package depend
name_dotprj=nopack_depend_editor
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp-Editor.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor \
--skips 'Assembly-CSharp.csproj'

# build and copy dlls to unity project
DotBuild $path_sln/$name_dotprj $path_out_dot
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins nopack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins nopack_depend_player true



#---------------------------------------------------------------------------------------------------
#                                               install unity package, regenerate unity csproj files
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.pack.Packer.Add --packs com.unity.addressables --packs com.unity.2d.sprite
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.CommandLineExecuteMethods.GenU3DProjectFiles



#---------------------------------------------------------------------------------------------------
#                                        dotnet classlib project, unity plugin, depend unity package
name_dotprj=pack_depend_player
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor



#---------------------------------------------------------------------------------------------------
#                                 dotnet classlib project, unity editor plugin, unity package depend
name_dotprj=pack_depend_editor
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_editor
DotProjectAddReference $path_sln/$name_dotprj $path_sln/pack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp-Editor.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor \
--skips 'Assembly-CSharp.csproj'

# build and copy dlls to unity project
DotBuild $path_sln/$name_dotprj $path_out_dot
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins pack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins pack_depend_player true



#---------------------------------------------------------------------------------------------------
# TODO: edit DLLs' unity meta file
# TODO: change 'isExplicitlyReferenced: 0'
# TODO:     to 'isExplicitlyReferenced: 1'
# TODO: or open unity, select the dll, in then dll inspector, uncheck 'Auto Reference'



#---------------------------------------------------------------------------------------------------
#                                                                      regenerate unity csproj files
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.CommandLineExecuteMethods.GenU3DProjectFiles
