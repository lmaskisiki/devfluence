function agent(name,ipAddress, port) {
    let _name= name;
    let _ipAddress= ipAddress;
    let _port=port;
    return{
        name:_name,
        ipAddress:_ipAddress,
        port:_port
    }
}