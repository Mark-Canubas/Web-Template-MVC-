function toggleSidebar() {
    var sidebar = document.getElementById('sidebarMenu');
    sidebar.classList.toggle('hidden');
}

document.addEventListener('DOMContentLoaded', function () {
    var collapseElements = document.querySelectorAll('.collapse');

    collapseElements.forEach(function (collapseEl) {
        collapseEl.addEventListener('show.bs.collapse', function () {
            var button = document.querySelector('[data-bs-target="#' + collapseEl.id + '"]');
            var chevron = button.querySelector('.bi-chevron-down');
            if (chevron) {
                chevron.classList.remove('bi-chevron-down');
                chevron.classList.add('bi-chevron-up');
            }
        });

        collapseEl.addEventListener('hide.bs.collapse', function () {
            var button = document.querySelector('[data-bs-target="#' + collapseEl.id + '"]');
            var chevron = button.querySelector('.bi-chevron-up');
            if (chevron) {
                chevron.classList.remove('bi-chevron-up');
                chevron.classList.add('bi-chevron-down');
            }
        });
    });
});
