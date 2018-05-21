## Milestone 3 user stories 3.1

As a user, I want to be able to add a agent, so that I can execute tasks against that agent.
		Given that I have added an agent, I should be able to see the status of that agent, so that I know whether the agent is online or offline.

As a user, I want to be able to update the agent, to ensure the correct information.
When  updating an agent, 
		Given that the status indicates that the agent is not healthy, the error message should be displayed on the page
		Given that the status indicates that the agent is healthy, should update the agent details


As a user, I want to be able to remove agent, so that I can that exclude agents that are no longer needed.

## Milestone 3 user stories 3.2

As a User, I want to be able to execute a command against an agent, so that I can get output specific to the target agent
When executing a command, I want to see the output on the dashboard
	** Given that I select ip command, I should get the ip address of the agent.
	** Given that I select os command, I should get the operating system of the agent
	*Given that I select hostname command, I should get the hostname of the agent
	* Given that I select hostname command, with fully-qualified option, I should get a  fully qualified hostname of the agent
	* Given that I select script command, I should be able enter the script text 
		* Then execute it against the agent And display the output


As a User, I want to be able to execute a command against multiple agents, so that I can get output from  different agents at the same time.

