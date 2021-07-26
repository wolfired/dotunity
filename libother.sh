function PackerPack() {
    TexturePacker.exe \
    --data E:/workspace_u3d/addrsrc/Assets/Addrable/UITextures/bagui.xml \
    --format xml \
    --size-constraints POT \
    --pack-mode Best \
    --force-squared \
    E:/workspace_u3d/addrsrc/Assets/Addrable/UITextures/.bagui

    TexturePacker.exe \
    --data E:/workspace_u3d/addrsrc/Assets/Addrable/UITextures/commonui.xml \
    --format xml \
    --size-constraints POT \
    --pack-mode Best \
    --force-squared \
    E:/workspace_u3d/addrsrc/Assets/Addrable/UITextures/.commonui
}
