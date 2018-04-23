// Add an empty item to a combo box if necessary. In the example this approach is not used
function OnInit(s, e) {
    s.InsertItem(0, 'Add New Item', -1);
}
// show a popup to edit a combo box datasource
function OnComboProduct_ButtonClick(s, e) {
    popup.ShowAtElement(s.GetMainElement());
}

var command = "";
function OnGridProducts_BeginCallback(s, e) {
    command = e.command;
}
// update the main grid datasource
function OnGridProducts_EndCallback(s, e) {
    if (command == "UPDATEEDIT") {
        gridView.Refresh();
    }
}