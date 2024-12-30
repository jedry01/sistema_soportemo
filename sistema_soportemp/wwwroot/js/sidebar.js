const sidebar = document.getElementById('sidebar');
const content = document.getElementById('content');
let isSidebarCollapsed = true;

// Manejo de hover solo después del primer clic
sidebar.addEventListener('mouseenter', () => {
    if (isSidebarCollapsed) {
        sidebar.classList.remove('collapsed');
    }
});

sidebar.addEventListener('mouseleave', () => {
    if (isSidebarCollapsed) {
        sidebar.classList.add('collapsed');
    }
});

// Eliminar la clase collapsed al cargar la página
window.addEventListener('load', () => {
    sidebar.classList.remove('collapsed');
    isSidebarCollapsed = true;
});