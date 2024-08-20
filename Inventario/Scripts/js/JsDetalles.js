/**
    * Función para ordenar las columnas de la tabla.

    */
function sortTable(columnIndex, isNumeric = false) {
    const table = document.querySelector('#divLista .table');
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    const currentDirection = table.getAttribute('data-sort-direction') || 'asc';
    const newDirection = currentDirection === 'asc' ? 'desc' : 'asc';

    rows.sort((a, b) => {
        const aText = a.querySelector(`td:nth-child(${columnIndex + 1})`).textContent.trim();
        const bText = b.querySelector(`td:nth-child(${columnIndex + 1})`).textContent.trim();

        if (isNumeric) {
            return newDirection === 'asc' ? parseFloat(aText) - parseFloat(bText) : parseFloat(bText) - parseFloat(aText);
        } else {
            return newDirection === 'asc' ? aText.localeCompare(bText) : bText.localeCompare(aText);
        }
    });

    table.setAttribute('data-sort-direction', newDirection);
    tbody.innerHTML = '';
    rows.forEach(row => tbody.appendChild(row));
}

/**
 * Añade eventos de ordenación a los encabezados de columna.
 */
function attachSortEvents() {
    document.querySelectorAll('#divLista .table th').forEach((th, index) => {
        th.addEventListener('click', () => {
            const isNumeric = th.classList.contains('numeric-column');
            sortTable(index, isNumeric);
        });
    });
}

/**
 * Función para filtrar filas por columna específica.

 */
function filterColumn(columnIndex) {
    const filterValue = document.querySelectorAll('.column-filter')[columnIndex].value.toLowerCase();
    const rows = document.querySelectorAll('#divLista .table tbody tr');

    rows.forEach(row => {
        const cell = row.querySelectorAll('td')[columnIndex];
        const cellText = cell.textContent.toLowerCase();
        row.style.display = cellText.includes(filterValue) ? '' : 'none';
    });
}

/**
 * Añade eventos de entrada a los campos de filtro.
 */
function attachFilterEvents() {
    document.querySelectorAll('.column-filter').forEach((input, index) => {
        input.addEventListener('input', () => filterColumn(index));
    });
}

document.addEventListener('DOMContentLoaded', () => {
    attachSortEvents();
    attachFilterEvents();
});
