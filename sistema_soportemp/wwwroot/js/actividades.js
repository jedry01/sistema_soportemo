// Calendar initialization
function initializeCalendar() {
    const calendarEl = document.getElementById('calendar');
    if (!calendarEl) return;

    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: 'es',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: '/Actividades/GetActividades',
        eventClick: (info) => showActivityDetails(info.event),
        eventClassNames: (arg) => {
            return arg.event.extendedProps.completed ? ['completed-activity'] : [];
        }
    });

    calendar.render();
}

// Activity details modal
function showActivityDetails(event) {
    const data = event.extendedProps;
    const formattedDate = new Date(event.start).toLocaleString('es-ES', {
        dateStyle: 'medium',
        timeStyle: 'short'
    });

    Swal.fire({
        title: event.title,
        html: `
            <div class="text-left">
                <p><strong>Técnico:</strong> ${data.trabajador}</p>
                <p><strong>Ubicación:</strong> ${data.ubicacion}</p>
                <p><strong>Fecha:</strong> ${formattedDate}</p>
                <p><strong>Descripción:</strong> ${data.description || 'Sin descripción'}</p>
                <p><strong>Estado:</strong> ${data.completed ? 'Completada' : 'Pendiente'}</p>
            </div>
        `,
        showCancelButton: true,
        confirmButtonText: data.completed ? 'Marcar como Pendiente' : 'Marcar como Completada',
        cancelButtonText: 'Cerrar',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#6c757d'
    }).then((result) => {
        if (result.isConfirmed) {
            toggleActivityStatus(event.id);
        }
    });
}

// Toggle activity status
async function toggleActivityStatus(id) {
    try {
        const response = await fetch(`/Actividades/ToggleComplete/${id}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        if (response.ok) {
            location.reload();
        } else {
            throw new Error('Error al actualizar la actividad');
        }
    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'No se pudo actualizar el estado de la actividad'
        });
    }
}

// Initialize calendar when DOM is loaded
document.addEventListener('DOMContentLoaded', initializeCalendar);