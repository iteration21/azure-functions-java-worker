package com.microsoft.azure.serverless.functions;

public class HttpResponseMessage {
    public HttpResponseMessage(int status) {
        this(status, null);
    }

    public HttpResponseMessage(Object body) {
        this(200, body);
    }

    public HttpResponseMessage(int status, Object body) {
        this.status = status;
        this.body = (body != null ? body : "");
    }

    public int getStatus() { return this.status; }
    public Object getBody() { return this.body; }

    private int status;
    private Object body;
}