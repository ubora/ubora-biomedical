Feature: Menu Options page
    As a signed in user
    I want to click Menu button and on all options in the Menu

Background:
    Given I go to Home page

Scenario: I sign in and click View profile
    When I sign in as user
        And I click on the element "span=Menu"
        And I click on the element "a=View profile"
        And I wait for the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I click My projects
    When I click on the element "span=Menu"
        And I click on the element "i=folder"
    Then I expect the title of the page "View projects - UBORA"

Scenario: I click New projects
    When I click on the element "span=Menu"
        And I click on the element "i=create_new_folder"
    Then I expect the title of the page "Project drafting - UBORA"

Scenario: I click Notifications
    When I click on the element "span=Menu"
        And I click on the element "i=notifications"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "i=history"
    Then I expect the title of the page "Notification history - UBORA"
    When I click on the element ".notifications_pending-button"
    Then I expect the title of the page "Notifications - UBORA"

Scenario: I click Log out
    When I log out
    Then I expect the title of the page "Welcome - UBORA"