Feature: User email changes
    As a user
    I want to register an account and change my email

Background:
    Given I go to Home page

Scenario: I try to change my password to empty password
    When I click on the element "#SignInSignUp"
        And I sign up as "change@email.eu"
        And I click on the element "span=Menu"
        And I click on the element "a=View profile"
        And I click on the element "a=Change email"
    Then I expect the title of the page "Change Email - UBORA"
    When I click on the element "button=Change email"
    Then I expect the element "span=The Email field is required." is visible
        And I expect the element "span=The Password field is required." is visible
        And I expect the title of the page "Change Email - UBORA"

Scenario: I change my password
    When I click on the element "span=Menu"
        And I click on the element "a=View profile"
        And I click on the element "a=Change email"
        And I set value "email@change.eu" to the element "#NewEmail"
        And I set value "Test12345" to the element "#Password"
        And I click on the element "button=Change email"
    Then I expect the title of the page "Manage your account - UBORA"
        And I expect the element "p=Email was changed successfully!" is visible

Scenario: I log out
    When I log out
    Then I expect the title of the page "Welcome - UBORA"
        And I expect the element "#SignInSignUp" is visible

Scenario: I log in with my changed password
    When I sign in as "email@change.eu" with password "Test12345"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=firstName lastName" is visible