function abstractAjax(url, method, data, successCallback, errorCallback) {
  $.ajax({
    url: url,
    type: method, 
    data: data, 
    dataType: 'json', 
    success: function(response) {
      if (successCallback && typeof successCallback === 'function') {
        successCallback(response);
      }
    },
    error: function(xhr, status, error) {
      if (errorCallback && typeof errorCallback === 'function') {
        errorCallback(xhr, status, error);
      }
    }
  });
}

export default abstractAjax;




