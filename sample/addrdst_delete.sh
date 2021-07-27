#!/bin/env bash

source $(dirname $0)/addrdst_inc.sh



#---------------------------------------------------------------
DotSolutionDelete $path_sln $path_out_dot

name_dotprj=u3dcsprojcloner
DotProjectDelete $path_sln/$name_dotprj

name_dotprj=nopack_depend_player
DotProjectDelete $path_sln/$name_dotprj

name_dotprj=nopack_depend_editor
DotProjectDelete $path_sln/$name_dotprj

name_dotprj=pack_depend_player
DotProjectDelete $path_sln/$name_dotprj

name_dotprj=pack_depend_editor
DotProjectDelete $path_sln/$name_dotprj
