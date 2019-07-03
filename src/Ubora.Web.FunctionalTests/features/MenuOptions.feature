Feature: Menu Options 
    As a signed in user
    I want to click Menu button and on all options in the Menu

Background:
    Given I go to Home page

Scenario: Check on console output
    Then I expect the console output to clear

Scenario: I sign in and click Profile
    When I sign in as user
        And I go to profile settings
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I click Projects
    When I click on the element "span=Projects"
    Then I expect the title of the page "Medical device projects - UBORA"

Scenario: I click New projects
    When I click on the element "a=Create project"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I click Notifications
    When I click on notifications
    Then I expect the title of the page "Notifications - UBORA"

Scenario: I click Sign out
    When I log out
    Then I expect the title of the page "UBORA"