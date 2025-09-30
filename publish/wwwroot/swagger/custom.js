window.onload = function () {
    const ui = SwaggerUIBundle({
        url: "/swagger/v1/swagger.json",
        dom_id: '#swagger-ui',
        requestInterceptor: (req) => {
            req.headers['ngrok-skip-browser-warning'] = 'true';
            return req;
        }
    });
};
