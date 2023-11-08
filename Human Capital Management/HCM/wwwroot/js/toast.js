import Toastify from '../toastify-js/toastify';

function showSuccessToast(data) {
    Toastify({
        text: data,
        duration: 3000,
        newWindow: true,
        close: true,
        gravity: "top",
        position: "right",
        stopOnFocus: true,
        style: {
            background: "linear-gradient(to right, #00b09b, #96c93d)",
        },
    }).showToast();
}




export {showSuccessToast};
