Feature: Sign In
	As an authorised editor
	I want to sign in to the website
	So I can manage the content

Background: 
	Given the website is running
	And I am not signed in

@wip
Scenario: Authorised editor signs in with Google account
	Given I am an authorised editor
	And I have a Google account
	When I visit the sign in page
	Then I can see the Google sign in page
	When I complete the Google sign in page
	Then I can see the admin page
