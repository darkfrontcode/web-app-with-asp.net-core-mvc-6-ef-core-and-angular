// site.js
(function startup() {

	var sidebar = document.getElementById("sidebar");
	var wrapper = document.getElementById("wrapper");
	var toggleList = new Array(sidebar, wrapper);

	var sidebarToggle = document.getElementById("sideBarToggle");

	sidebarToggle.onclick = function(){
		toggleList.forEach(function(item) {
            item.classList.toggle("hide-sidebar");
            var hasSidebar = item.classList.contains("hide-sidebar");
            sidebarToggle.innerHTML = hasSidebar ? "Show Sidebar" : "hide Sidebar";
        });
	}

})();
