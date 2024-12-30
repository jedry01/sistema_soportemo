document.addEventListener('DOMContentLoaded', function () {
    var dropdownToggles = document.querySelectorAll('.dropdown-toggle');
    dropdownToggles.forEach(function (toggle) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();
            var target = document.querySelector(this.getAttribute('data-bs-target'));
            if (target) {
                if (target.classList.contains('show')) {
                    var collapse = bootstrap.Collapse.getInstance(target) || new bootstrap.Collapse(target);
                    collapse.hide();
                } else {
                    var collapse = bootstrap.Collapse.getInstance(target) || new bootstrap.Collapse(target);
                    collapse.show();
                }
            }
        });
    });
});