document.addEventListener('DOMContentLoaded', function () {
    const themeToggle = document.getElementById('themeToggle');
    const icon = themeToggle.querySelector('i');
    const themeLabel = document.getElementById('themeLabel');

    // Cargar tema guardado
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        document.body.classList.add('dark-theme');
        icon.classList.replace('fa-moon', 'fa-sun');
        themeLabel.textContent = 'Modo Claro';
    } else {
        themeLabel.textContent = 'Modo Oscuro'; // Asegúrate de establecer el texto inicial
    }

    themeToggle.addEventListener('click', function () {
        document.body.classList.toggle('dark-theme');

        if (document.body.classList.contains('dark-theme')) {
            icon.classList.replace('fa-moon', 'fa-sun');
            themeLabel.textContent = 'Modo Claro';
            localStorage.setItem('theme', 'dark');
        } else {
            icon.classList.replace('fa-sun', 'fa-moon');
            themeLabel.textContent = 'Modo Oscuro';
            localStorage.setItem('theme', 'light');
        }
    });
});
