document.addEventListener('DOMContentLoaded', function () {
    const activoSelect = document.getElementById('ActivoId');
    const marcaSelect = document.getElementById('MarcaId');
    const modeloSelect = document.getElementById('ModeloId');

    activoSelect.addEventListener('change', async function () {
        const activoId = this.value;

        console.log(`Activo seleccionado: ${activoId}`);

        // Resetear los selects dependientes
        marcaSelect.innerHTML = '<option value="">Selecciona una marca</option>';
        modeloSelect.innerHTML = '<option value="">Selecciona un modelo</option>';

        if (!activoId) return;

        try {
            // Obtener marcas filtradas por activo con manejo de errores
            const response = await fetch(`/ReparacionEquipos/GetMarcasPorActivo/${activoId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const marcas = await response.json();

            if (!marcas || marcas.length === 0) {
                console.log('No hay marcas disponibles para el activo seleccionado.');
                marcaSelect.innerHTML = '<option value="">No hay marcas disponibles</option>';
                return;
            }

            console.log(`Marcas recibidas: `, marcas);

            marcas.forEach(marca => {
                const option = new Option(marca.nombre, marca.id);
                marcaSelect.add(option);
            });

            // Habilitar el select de marcas
            marcaSelect.disabled = false;
        } catch (error) {
            console.error('Error al cargar las marcas:', error);
            marcaSelect.innerHTML = '<option value="">Error al cargar marcas</option>';
        }
    });

    marcaSelect.addEventListener('change', async function () {
        const marcaId = this.value;
        const activoId = activoSelect.value;

        console.log(`Marca seleccionada: ${marcaId}`);
        console.log(`Activo actual: ${activoId}`);

        // Resetear el select de modelos
        modeloSelect.innerHTML = '<option value="">Selecciona un modelo</option>';

        if (!marcaId || !activoId) return;

        try {
            // Obtener modelos filtrados por marca y activo con manejo de errores
            const response = await fetch(`/ReparacionEquipos/GetModelosPorMarcaYActivo?marcaId=${marcaId}&activoId=${activoId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const modelos = await response.json();

            if (!modelos || modelos.length === 0) {
                console.log('No hay modelos disponibles para la marca y activo seleccionados.');
                modeloSelect.innerHTML = '<option value="">No hay modelos disponibles</option>';
                return;
            }

            console.log(`Modelos recibidos: `, modelos);

            modelos.forEach(modelo => {
                const option = new Option(modelo.nombre, modelo.id);
                modeloSelect.add(option);
            });

            // Habilitar el select de modelos
            modeloSelect.disabled = false;
        } catch (error) {
            console.error('Error al cargar los modelos:', error);
            modeloSelect.innerHTML = '<option value="">Error al cargar modelos</option>';
        }
    });

    modeloSelect.addEventListener('change', function () {
        console.log(`Modelo seleccionado: ${this.value}`);
        // No es necesario hacer nada aquí
    });
});
