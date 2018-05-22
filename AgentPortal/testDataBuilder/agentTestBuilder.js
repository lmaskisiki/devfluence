function agentTestBuilder() {
    let _name;
    let _ipAddress;
    let _port;

    function withName(name) {
        this._name = name;
        return this;
    }

    function withIpAddress(ipAddress) {
        this._ipAddress = ipAddress;
        return this;
    }
    function withPort(portNumber) {
        this._port = portNumber;
        return this;
    }

    function build() {
        return new agent(this._name, this._ipAddress, this._port);
    }

    return {
        withName: withName,
        withIpAddress: withIpAddress,
        withPort, withPort,
        build: build
    }
}