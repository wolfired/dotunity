#!/bin/env bash

# root=$(dirname $(readlink -f $0)) # Used in Linux
root=E:/workspace_git/dotunity # Used in Window
root_name=$(basename $root)

source $root/libdot.sh
source $root/libunity.sh
source $root/libother.sh

path_sln=$root
name_sln=$root_name
path_out_dot=${path_out_dot:-$path_sln/out_dot}
path_out_u3d=${path_out_u3d:-$path_sln/out_u3d}
path_u3dprj=${path_u3dprj:-'E:/workspace_u3d/addrsrc'}



#---------------------------------------------------------------
DotSolutionClean $path_sln $path_out_dot

name_dotprj=u3dcsprojcloner
DotProjectClean $path_sln/$name_dotprj

name_dotprj=nopack_depend_player
DotProjectClean $path_sln/$name_dotprj

name_dotprj=nopack_depend_editor
DotProjectClean $path_sln/$name_dotprj

name_dotprj=pack_depend_player
DotProjectClean $path_sln/$name_dotprj

name_dotprj=pack_depend_editor
DotProjectClean $path_sln/$name_dotprj
