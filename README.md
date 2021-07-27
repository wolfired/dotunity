# Dotunity

Dotnet Unity Onekey Shellscript.

# Tested Unity Version

* `2021.2.0a21`

# Sample Scripts

`sample/addrsrc_*.sh`, `sample/addrdst_*.sh` are samples.

> `sample/addrsrc_*.sh`: use for unity project exist   
> `sample/addrdst_*.sh`: use for not exist   

* `*_inc.sh`: settings

```bash
# path of this project
root_path=
# path of unity project you want to create
path_u3dprj=
```

* `*_new.sh`: create a new unity project, and bind dotnet project to it.

* `*_build.sh`: build dotnet project, and copy dlls to unity project.

* `*_clean.sh`: clean dotnet project.

* `*_delete.sh`: delete dotnet project.

# Sample Dotnet projects

* `nopack_depend_player`, game logic, no unity package depend

* `nopack_depend_editor`, unity editor helper, no unity package depend, depend `nopack_depend_player`

* `pack_depend_player`, game logic, depend unity packages, depend `nopack_depend_player`

* `pack_depend_editor`, unity editor helper, depend unity package, depend `nopack_depend_player` `nopack_depend_editor` `pack_depend_player`
