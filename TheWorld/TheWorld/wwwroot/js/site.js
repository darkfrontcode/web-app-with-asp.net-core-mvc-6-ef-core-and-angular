// site.js
(function startup() {

    var sidebar = document.getElementById("sidebar");
    var wrapper = document.getElementById("wrapper");
    var toggleList = new Array(sidebar, wrapper);

    var sidebarToggle = document.getElementById("sideBarToggle");

    sidebarToggle.onclick = function () {
        toggleList.forEach(function (item) {

            item.classList.toggle("hide-sidebar");

            var hasSidebar = item.classList.contains("hide-sidebar");
            var direction = hasSidebar ? "glyphicon-chevron-right" : "glyphicon-chevron-left";
            var className = "glyphicon " + direction;

            sidebarToggle.querySelector("i").setAttribute("class", className);

        });
    }

})();
