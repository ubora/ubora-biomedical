Feature: User password changes
    As a user
    I want to register an account and change my password

Background:
    Given I go to Home page

Scenario: I try to change my password to empty password
    When I click on the element "#SignInSignUp"
        And I sign up as "change@password.eu"
        And I click on the element "span=Menu"
        And I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Change password"
    Then I expect the title of the page "Change Password - UBORA"
    When I click on the element "button=Change password"
    Then I expect the element "span=The Old password field is required." is visible
        And I expect the element "span=The New password field is required." is visible
        And I expect the title of the page "Change Password - UBORA"

Scenario: I change my password
    When I click on the element "span=Menu"
        And I wait for the element "a=View profile"
        And I click on the element "a=View profile"
        And I click on the element "a=Change password"
        And I set value "Test12345" to the element "#OldPassword"
        And I set value "Test1234" to the element "#NewPassword"
        And I set value "Test1234" to the element "#ConfirmPassword"
        And I click on the element "button=Change password"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I sign out
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"
        And I expect the element "#SignInSignUp" is visible

Scenario: I sign in with my changed password
    When I sign in as "change@password.eu" with password "Test1234"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=Change Password" is visible
