function agent(name, ipAddress, port) {
    let _service = new agentService();
    let _name = name;
    let _ipAddress = ipAddress;
    let _port = port;
    let _active = false;
    return {
        name: _name,
        ipAddress: _ipAddress,
        port: _port,
        active: _active,
        executions: [],
        getExecutions: function (doneFn, erroFn) {
            return _service.getExecutions(this,doneFn, erroFn);
        },
        canContact: function (doneFn, erroFn) {
            _service.ping(this, doneFn, erroFn);
        }
    }
}