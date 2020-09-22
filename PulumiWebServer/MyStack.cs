using Pulumi;
using Pulumi.Aws;
using Pulumi.Aws.Ec2;
using Pulumi.Aws.Ec2.Inputs;
using Pulumi.Aws.Inputs;

class MyStack : Stack
{
    public MyStack()
    {
        var ami = Output.Create(GetAmi.InvokeAsync(new GetAmiArgs
        {
            Filters =
                {
                    new GetAmiFilterArgs
                    {
                        Name = "name",
                        Values =  { "amzn-ami-hvm-*" },
                    },
                },
            Owners = { "137112412989" }, // This owner ID is Amazon
            MostRecent = true,
        }));

var group = new SecurityGroup("webserver-secgrp", new SecurityGroupArgs
{
    Ingress =
    {
        new SecurityGroupIngressArgs
        {
            Protocol = "tcp",
            FromPort = 22,
            ToPort = 22,
            CidrBlocks = { "0.0.0.0/0" },
        },
        new SecurityGroupIngressArgs
        {
            Protocol = "tcp",
            FromPort = 80,
            ToPort = 90,
            CidrBlocks = { "0.0.0.0/0" },
        },
    },
});
        var userData = @"
                    #!/bin/bash
                    echo ""Hello, World!"" > index.html
                    nohup python -m SimpleHTTPServer 80 &
                    ";

        var server = new Instance("webserver-www", new InstanceArgs
        {
            InstanceType = Size,
            VpcSecurityGroupIds = { group.Id }, // reference the security group resource above
            UserData = userData,
            Ami = ami.Apply(x => x.Id),
        }); ;

        PublicIp = server.PublicIp;
        PublicDns = server.PublicDns;
    }

    [Output]
    public Output<string> PublicIp { get; set; }

    [Output]
    public Output<string> PublicDns { get; set; }

    private const string Size = "t2.micro"; // t2.micro is available in the AWS free tier
}