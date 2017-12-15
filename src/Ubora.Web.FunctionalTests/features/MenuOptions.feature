Feature: Menu Options 
    As a signed in user
    I want to click Menu button and on all options in the Menu

Background:
    Given I go to Home page

Scenario: I sign in and click Profile
    When I sign in as user
        And I click on the element "span=Profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I click My projects
    When I click on the element "span=Projects"
    Then I expect the title of the page "View projects - UBORA"

Scenario: I click New projects
    When I click on the element "p=Create new project"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I click Notifications
    When I click on the element "span=Notifications"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "span=Notification history"
    Then I expect the title of the page "Notification history - UBORA"
    When I click on the element "span=Notifications"
    Then I expect the title of the page "Notifications - UBORA"

Scenario: I click Sign out
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"