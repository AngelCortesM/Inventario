// Define URLs para operaciones relacionadas con productos
const urlNew = "@Url.Content("~/Producto/New")"; // URL para el formulario de nuevo producto
const urlGet = "@Url.Content("~/Producto/List")"; // URL para obtener la lista de productos
const urlEdit = "@Url.Content("~/Producto/Edit")"; // URL para el formulario de edición
const urlGetProveedor = "@Url.Action("GetProveedor", "Producto")"; // URL para obtener la lista de proveedores
const urlCreate = "@Url.Content("~/Producto/Save")"; // URL para guardar un nuevo producto

// Variable para rastrear la visibilidad del formulario 'Nuevo'
let isNewContentVisible = false;

/**
 * Función para manejar la visualización del formulario 'Nuevo'.
 */
function New() {
    const divNew = document.getElementById("divNew");

    if (isNewContentVisible) {
        // Verificar si hay texto en los campos de entrada
        const inputs = divNew.querySelectorAll('input');
        let hasText = false;
        inputs.forEach(input => {
            if (input.value.trim() !== "") {
                hasText = true;
            }
        });

        if (hasText) {
            // Si hay texto, muestra una alerta y no oculta el formulario
            alert("No puedes cerrar el formulario mientras haya texto en los campos.");
        } else {
            // Si no hay texto, oculta el formulario
            divNew.innerHTML = "";
            isNewContentVisible = false;
        }
    } else {
        // Si el contenido no está visible, lo carga y muestra
        fetch(urlNew)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
                return response.text();
            })
            .then(data => {
                divNew.innerHTML = data;
                isNewContentVisible = true;

                // Ejecuta el script para cargar los proveedores después de cargar el contenido
                loadProveedores();
            })
            .catch(error => {
                console.error('There has been a problem with your fetch operation:', error);
            });
    }
}
/**
* Función para manejar la visualización del formulario de edición.

*/
function Edit(id) {
    const urlEdit = `@Url.Content("~/Producto/Edit/")${id}`;
    fetch(urlEdit)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            return response.text();
        })
        .then(data => {
            document.getElementById("divEdit").innerHTML = data;

            // Ejecuta el script para cargar los proveedores después de cargar el contenido
            loadProveedores();
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

/**
 * Función para cerrar el formulario de edición.
 */
function EditC() {
    if (isNewContentVisible) {
        // Verificar si hay texto en los campos de entrada
        const inputs = divNew.querySelectorAll('input');
        let hasText = false;
        inputs.forEach(input => {
            if (input.value.trim() !== "") {
                hasText = true;
            }
        });

        if (hasText) {
            // Si hay texto, muestra una alerta y no oculta el formulario
            alert("No puedes cerrar el formulario mientras haya texto en los campos.");
        } else {
            // Si no hay texto, oculta el formulario
            divNew.innerHTML = "";
            isNewContentVisible = false;
        }
    } else {
        // Limpiar el contenido del divEdit para cerrar el formulario de edición
        document.getElementById("divEdit").innerHTML = "";
    }
}

/**
 * Función para cerrar el formulario de nuevo proveedor.
 */
function NewC() {
    const divNew = document.getElementById("divNew");

    // Verificar si hay texto en los campos de entrada dentro de divNew
    const inputs = divNew.querySelectorAll('input');
    let hasText = false;
    inputs.forEach(input => {
        if (input.value.trim() !== "") {
            hasText = true;
        }
    });

    if (hasText) {
        // Si hay texto, muestra una alerta de confirmación
        Swal.fire({
            title: '¿Estás seguro?',
            text: "Tienes texto sin guardar. ¿Estás seguro de que deseas cerrar el formulario?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, cerrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                // Si el usuario confirma, oculta el formulario
                divNew.innerHTML = "";
            }
        });
    } else {
        // Si no hay texto, oculta el formulario
        divNew.innerHTML = "";
    }
}

/**
 * Función para cargar la lista de productos.
 */
function Get() {
    fetch(urlGet)
        .then(response => response.text())
        .then(data => {
            document.getElementById("divLista").innerHTML = data;
            attachSortEvents(); // Añade eventos de clic a los encabezados después de cargar los datos
            attachFilterEvents(); // Añade eventos de filtro después de cargar los datos
        });
}

// Inicializa la tabla con datos y eventos
Get();

/**
* Función para realizar una búsqueda en toda la tabla.
*/
function search() {
    // Obtiene el valor del campo de búsqueda y lo convierte a minúsculas
    const query = document.getElementById('searchInput').value.toLowerCase();
    // Selecciona todas las filas de la tabla
    const rows = document.querySelectorAll('#divLista .table tbody tr');

    // Recorre cada fila
    rows.forEach(row => {
        // Selecciona todas las celdas de la fila actual
        const cells = row.querySelectorAll('td');
        // Inicializa una variable para saber si hay coincidencias
        let match = false;

        // Recorre cada celda de la fila
        cells.forEach(cell => {
            // Si el texto de la celda contiene la consulta de búsqueda
            if (cell.textContent.toLowerCase().includes(query)) {
                match = true; // Marca que hay coincidencia
            }
        });

        // Muestra la fila si hay coincidencia, de lo contrario la oculta
        row.style.display = match ? '' : 'none';
    });
}

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

/**
 * Función para cargar proveedores en el select.
 */
// Función para cargar proveedores en el menú desplegable
function loadProveedores() {
    const proveedorList = document.getElementById("Proveedor_list");
    if (!proveedorList) {
        console.error('El elemento de la lista de proveedores no se encontró.');
        return;
    }

    fetch(urlGetProveedor)
        .then(response => response.json())
        .then(data => {
            proveedorList.innerHTML = ''; // Limpia opciones existentes
            data.forEach(proveedor => {
                const li = document.createElement("li");
                li.textContent = proveedor.Nombre;
                li.dataset.id = proveedor.Id;
                li.onclick = function () {
                    selectProveedor(proveedor.Id, proveedor.Nombre);
                };
                proveedorList.appendChild(li);
            });
        })
        .catch(error => console.error('Error fetching proveedores:', error));
}

// Función para filtrar y mostrar las opciones en el menú desplegable
function filterProveedores() {
    const searchInput = document.getElementById("Proveedor_search").value.toLowerCase();
    const proveedorList = document.getElementById("Proveedor_list");

    if (!proveedorList) {
        console.error('El elemento de la lista de proveedores no se encontró.');
        return;
    }

    // Muestra el menú de opciones
    proveedorList.style.display = searchInput ? 'block' : 'none';

    // Filtra las opciones del menú
    Array.from(proveedorList.children).forEach(li => {
        const text = li.textContent.toLowerCase();
        li.style.display = text.includes(searchInput) ? 'block' : 'none';
    });
}

// Función para seleccionar un proveedor del menú desplegable
function filterProveedores() {
    const searchInput = document.getElementById('Proveedor_search');
    const list = document.getElementById('Proveedor_list');
    const query = searchInput.value.toLowerCase();

    if (query.length === 0) {
        list.style.display = 'none';
        return;
    }

    fetch(urlGetProveedor)
        .then(response => response.json())
        .then(data => {
            list.innerHTML = '';
            const filteredData = data.filter(proveedor => proveedor.Nombre.toLowerCase().includes(query));

            if (filteredData.length > 0) {
                filteredData.forEach(proveedor => {
                    const listItem = document.createElement('li');
                    listItem.textContent = proveedor.Nombre;
                    listItem.dataset.id = proveedor.Id; // Almacena el ID en el data attribute
                    listItem.onclick = function () {
                        selectProveedor(proveedor.Id, proveedor.Nombre);
                    };
                    list.appendChild(listItem);
                });
                list.style.display = 'block';
            } else {
                list.style.display = 'none';
            }
        })
        .catch(error => console.error('Error fetching proveedores:', error));
}

function selectProveedor(id, nombre) {
    document.getElementById('Proveedor_search').value = nombre;
    document.getElementById('Proveedor_id').value = id; // Almacena el ID en el campo oculto
    document.getElementById('Proveedor_list').style.display = 'none';
}

// Oculta la lista si el usuario hace clic fuera de ella
document.addEventListener('click', function (e) {
    const list = document.getElementById('Proveedor_list');
    if (!list.contains(e.target) && e.target.id !== 'Proveedor_search') {
        list.style.display = 'none';
    }
});

// Cargar proveedores al inicio
document.addEventListener("DOMContentLoaded", function () {
    loadProveedores();
});

/**
 * Función para agregar un nuevo producto.
 */
function jsAdd() {
    fetch(urlCreate, {
        method: "POST",
        body: JSON.stringify({
            Cantidad: document.getElementById("Cantidad").value,
            Proveedor_id: document.getElementById("Proveedor_id").value,
            Valor: document.getElementById("Valor").value,
            Descripcion: document.getElementById("Descripcion").value,
            Estado: document.getElementById("Estado").value,
            Fecha: document.getElementById("Fecha").value,
            Nombre: document.getElementById("Nombre").value,
            Barras: document.getElementById("Barras").value,
            Categoria: document.getElementById("Categoria").value,
            Costo: document.getElementById("Costo").value
        }),
        headers: {
            'Accept': "application/json",
            "Content-Type": "application/json"
        }
    }).then(function (response) {
        if (response.ok) {
            return response.text();
        } else {
            throw new Error("Error al ejecutar: " + response.statusText);
        }
    }).then(function (data) {
        if (data != "1") {
            alert(data);
        } else {
            document.location.href = "@Url.Content("~/Producto/")";
        }
    }).catch(function (error) {
        console.error('Error:', error);
        alert("Se produjo un error: " + error.message);
    });
}

/**
* Función para editar un producto existente.
*/
// URL para actualizar un producto
var urlUpdate = "@Url.Content("~/Producto/Update")";

function jsEdit() {
    fetch(urlUpdate, {
        method: "POST",
        body: JSON.stringify({
            Cantidad: document.getElementById("Cantidad").value,
            Proveedor_id: document.getElementById("Proveedor_id").value,
            Valor: document.getElementById("Valor").value,
            Descripcion: document.getElementById("Descripcion").value,
            Estado: document.getElementById("Estado").value,
            Fecha: document.getElementById("Fecha").value,
            Nombre: document.getElementById("Nombre").value,
            Barras: document.getElementById("Barras").value,
            Categoria: document.getElementById("Categoria").value,
            Costo: document.getElementById("Costo").value,
            Id: document.getElementById("Id").value
        }),

        headers: {
            'Accept': "application/json",
            "Content-Type": "application/json"
        }
    }).then(function (response) {
        if (response.ok) {
            return response.text();
        } else {
            throw new Error("Error al ejecutar: " + response.statusText);
        }
    }).then(function (data) {
        if (data != "1") {
            alert(data);
        } else {
            document.location.href = "@Url.Content("~/Producto/")";
        }
    }).catch(function (error) {
        console.error('Error:', error);
        alert("Se produjo un error: " + error.message);
    });
}

/**
* Función para eliminar un proveedor.
* Muestra una confirmación antes de proceder con la eliminación.

*/
function jsDelete(Id) {
    // Mostrar un mensaje de confirmación
    Swal.fire({
        title: '¿Estás seguro?',
        text: "No podrás revertir esto",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#FF6F61',
        cancelButtonColor: '#00B0FF',
        confirmButtonText: 'Sí, eliminarlo',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            // Solo proceder si el usuario confirma la eliminación
            fetch("@Url.Content("~/Producto/Delete")", {
                method: "POST",
                body: JSON.stringify({ Id: Id }),
                headers: {
                    'Accept': "application/json",
                    "Content-Type": "application/json"
                }
            }).then(function (response) {
                if (response.ok) {
                    return response.text();
                } else {
                    throw new Error("Error al ejecutar: " + response.statusText);
                }
            }).then(function (data) {
                if (data != "1") {
                    Swal.fire({
                        title: 'Error',
                        text: data,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                } else {
                    Swal.fire({
                        title: 'Eliminado',
                        text: 'El registro ha sido eliminado.',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    });
                    Get(); // Actualiza la lista de proveedores
                }
            }).catch(function (error) {
                console.error('Error:', error);
                Swal.fire({
                    title: 'Error',
                    text: 'Se produjo un error: ' + error.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            });
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            // Opcional: Mostrar un mensaje de cancelación
            Swal.fire(
                'Cancelado',
                'El registro no fue eliminado',
                'info'
            );
        }
    });
}

//funcion apra exportar a excel
document.getElementById('exportToExcel').addEventListener('click', function () {
    const table = document.querySelector('#divLista .table');
    const wb = XLSX.utils.book_new();

    // Obtener las cabeceras desde los placeholders de los inputs
    const headers = Array.from(table.querySelectorAll('thead .header-input-container input')).map(input => input.placeholder.trim());
    console.log('Headers:', headers);

    // Obtener las filas de la tabla
    const rows = Array.from(table.querySelectorAll('tbody tr')).map(tr => {
        const cells = Array.from(tr.querySelectorAll('td')).map(td => td.innerText.trim());
        console.log('Row Cells:', cells); // Imprimir celdas de cada fila
        return cells;
    });

    // Verificar que se obtuvieron correctamente las cabeceras y las filas
    console.log('Rows:', rows);

    // Convertir en una hoja de cálculo
    const ws = XLSX.utils.aoa_to_sheet([headers, ...rows]);
    XLSX.utils.book_append_sheet(wb, ws, 'Clientes');

    // Exportar el archivo
    XLSX.writeFile(wb, 'tabla_clientes.xlsx');
});

// Función para exportar a PDF
document.getElementById('exportButton').addEventListener('click', function () {
    html2canvas(document.querySelector('#divLista')).then(canvas => {
        const imgData = canvas.toDataURL('image/png');
        const { jsPDF } = window.jspdf;
        const pdf = new jsPDF('p', 'mm', 'a4');

        // Agregar el logo
        const logoImg = '/Content/images/logo.png'; // Reemplaza con la ruta a tu imagen
        pdf.addImage(logoImg, 'PNG', 10, 10, 30, 30);

        // Agregar el nombre de la empresa
        pdf.setFontSize(13);
        pdf.text('On / Off Soluciones en linea', 50, 15);

        // Agregar otros detalles
        pdf.setFontSize(11);
        pdf.text('Dirección de la Empresa', 50, 20);
        pdf.text('Teléfono: (123) 456-7890', 50, 25);
        pdf.text('Email: contacto@empresa.com', 50, 30);

        // Agregar la tabla
        const imgWidth = 210; // Ancho de la página en mm
        const pageHeight = 295; // Altura de la página en mm
        const imgHeight = canvas.height * imgWidth / canvas.width;
        let heightLeft = imgHeight;

        let position = 42; // Ajusta la posición para que esté debajo del encabezado
        pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;

        while (heightLeft >= 0) {
            position = heightLeft - imgHeight;
            pdf.addPage();
            pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
            heightLeft -= pageHeight;
        }

        pdf.save('tabla.pdf');
    });
});